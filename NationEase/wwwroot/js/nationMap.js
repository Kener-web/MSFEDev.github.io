let googleMapsScript;
let map;
let infoWindow;
let markers = new Map();
let updateVersion = 0;

const contiguousUsBounds = {
    north: 49.5,
    south: 24.3,
    west: -125,
    east: -66.9
};

function loadGoogleMaps(apiKey) {
    if (window.google?.maps) {
        return Promise.resolve();
    }

    if (!googleMapsScript) {
        googleMapsScript = new Promise((resolve, reject) => {
            const script = document.createElement("script");
            script.src = `https://maps.googleapis.com/maps/api/js?key=${encodeURIComponent(apiKey)}&v=weekly`;
            script.async = true;
            script.onload = resolve;
            script.onerror = () => {
                googleMapsScript = undefined;
                reject(new Error("The Google Maps JavaScript API script failed to load."));
            };
            document.head.appendChild(script);
        });
    }

    return googleMapsScript;
}

export async function initializeMap(element, apiKey) {
    await loadGoogleMaps(apiKey);

    map = new window.google.maps.Map(element, {
        center: { lat: 39.8283, lng: -98.5795 },
        zoom: 4,
        mapTypeId: window.google.maps.MapTypeId.SATELLITE,
        mapTypeControl: false,
        streetViewControl: false,
        fullscreenControl: true
    });
    infoWindow = new window.google.maps.InfoWindow();
    map.fitBounds(contiguousUsBounds, 24);
}

function upsertMarker(city, location) {
    const key = String(city.cityId);
    let marker = markers.get(key);
    if (!marker) {
        marker = new window.google.maps.Marker({ map });
        markers.set(key, marker);
        marker.addListener("click", () => {
            const content = document.createElement("div");
            content.textContent = marker.getTitle();
            infoWindow.setContent(content);
            infoWindow.open({ map, anchor: marker });
        });
    }

    marker.setPosition(location);
    marker.setTitle(`[${city.stateAcronym}] ${city.name}`);
}

function fitSelectedCities() {
    const locations = Array.from(markers.values(), (marker) => marker.getPosition());
    if (locations.length === 0) {
        map.fitBounds(contiguousUsBounds, 24);
        return;
    }

    if (locations.length === 1) {
        map.setCenter(locations[0]);
        map.setZoom(7);
        return;
    }

    const bounds = new window.google.maps.LatLngBounds();
    for (const location of locations) {
        bounds.extend(location);
    }
    map.fitBounds(bounds, 48);
}

export async function updateMarkers(cities, coordinateReceiver) {
    const version = ++updateVersion;
    const cityIds = new Set(cities.map((city) => String(city.cityId)));
    const citiesWithCoordinates = new Set(cities
        .filter((city) => Number.isFinite(city.latitude) && Number.isFinite(city.longitude))
        .map((city) => String(city.cityId)));

    for (const [cityId, marker] of markers) {
        if (!cityIds.has(cityId) || !citiesWithCoordinates.has(cityId)) {
            window.google.maps.event.clearInstanceListeners(marker);
            marker.setMap(null);
            markers.delete(cityId);
        }
    }

    cities.forEach((city) => {
        if (Number.isFinite(city.latitude) && Number.isFinite(city.longitude)) {
            upsertMarker(city, { lat: city.latitude, lng: city.longitude });
        }
    });
    fitSelectedCities();

    const geocoder = new window.google.maps.Geocoder();
    let coordinateSaveFailures = 0;
    const citiesWithoutCoordinates = cities.filter(
        (city) => !Number.isFinite(city.latitude) || !Number.isFinite(city.longitude));

    let unresolvedCount = 0;
    for (const city of citiesWithoutCoordinates) {
        try {
            const response = await geocoder.geocode({
                address: `${city.name}, ${city.stateAcronym}, USA`,
                componentRestrictions: { country: "US" }
            });
            const resultLocation = response.results[0]?.geometry.location;
            if (!resultLocation) {
                unresolvedCount++;
                continue;
            }

            const location = { lat: resultLocation.lat(), lng: resultLocation.lng() };
            if (version !== updateVersion || !cityIds.has(String(city.cityId))) {
                continue;
            }

            try {
                await coordinateReceiver.invokeMethodAsync(
                    "SaveCityCoordinates",
                    city.cityId,
                    location.lat,
                    location.lng);
            } catch (error) {
                console.error(`Could not save coordinates for ${city.name}, ${city.stateAcronym}:`, error);
                coordinateSaveFailures++;
            }

            if (version === updateVersion && cityIds.has(String(city.cityId))) {
                upsertMarker(city, location);
            }
        } catch (error) {
            console.error(`Could not locate ${city.name}, ${city.stateAcronym}:`, error);
            unresolvedCount++;
        }
    }

    if (version !== updateVersion) {
        return {
            markerCount: markers.size,
            unresolvedCount: 0,
            coordinateSaveFailures: 0
        };
    }

    fitSelectedCities();
    return {
        markerCount: markers.size,
        unresolvedCount,
        coordinateSaveFailures
    };
}

export function dispose() {
    updateVersion++;
    if (window.google?.maps) {
        for (const marker of markers.values()) {
            window.google.maps.event.clearInstanceListeners(marker);
            marker.setMap(null);
        }
    }
    markers.clear();

    infoWindow?.close();
    infoWindow = undefined;

    if (map) {
        window.google.maps.event.clearInstanceListeners(map);
        map = undefined;
    }
}
