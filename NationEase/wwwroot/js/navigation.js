const pageControlSelector = [
    ".content a[href]",
    ".content button:not(:disabled)",
    ".content input:not(:disabled)",
    ".content select:not(:disabled)",
    ".content textarea:not(:disabled)",
    ".content [tabindex]:not([tabindex='-1'])"
].join(", ");

window.nationEaseKeyboard = window.nationEaseKeyboard || {};

window.nationEaseKeyboard.focusCityAction = (cityId) => {
    document.querySelector(`.content table tbody tr[data-city-id="${cityId}"] button`)?.focus();
};

window.nationEaseKeyboard.focusNationBuilderAction = (cityId) => {
    document.querySelector(`.content table tbody tr[data-city-id="${cityId}"] button:not(:disabled)`)?.focus();
};

window.nationEaseKeyboard.focusClearNationButton = () => {
    document.querySelector(".content .btn-warning")?.focus();
};

function getVisibleControls(container) {
    return Array.from(container.querySelectorAll(pageControlSelector))
        .filter((control) =>
            control.getClientRects().length > 0 &&
            !control.closest("[inert], [aria-hidden='true']")
        );
}

function findDirectionalControl(currentControl, controls, direction) {
    const currentBounds = currentControl.getBoundingClientRect();
    const currentX = currentBounds.left + currentBounds.width / 2;
    const currentY = currentBounds.top + currentBounds.height / 2;
    let nearestControl = null;
    let nearestScore = Infinity;

    for (const control of controls) {
        if (control === currentControl) {
            continue;
        }

        const bounds = control.getBoundingClientRect();
        const deltaX = bounds.left + bounds.width / 2 - currentX;
        const deltaY = bounds.top + bounds.height / 2 - currentY;
        const primaryDistance = direction === "ArrowLeft" || direction === "ArrowRight"
            ? deltaX * (direction === "ArrowRight" ? 1 : -1)
            : deltaY * (direction === "ArrowDown" ? 1 : -1);

        if (primaryDistance <= 1) {
            continue;
        }

        const secondaryDistance = direction === "ArrowLeft" || direction === "ArrowRight"
            ? Math.abs(deltaY)
            : Math.abs(deltaX);
        const score = primaryDistance + secondaryDistance * 2;

        if (score < nearestScore) {
            nearestControl = control;
            nearestScore = score;
        }
    }

    return nearestControl;
}

document.addEventListener("keydown", (event) => {
    if (event.altKey || event.ctrlKey || event.metaKey) {
        return;
    }

    const target = event.target;
    if (!(target instanceof Element)) {
        return;
    }

    const currentLink = target.closest(".sidebar nav a.nav-link");
    if (currentLink && (event.key === "ArrowDown" || event.key === "ArrowUp")) {
        const navLinks = Array.from(currentLink.closest("nav").querySelectorAll("a.nav-link"))
            .filter((link) => link.getClientRects().length > 0);
        const currentIndex = navLinks.indexOf(currentLink);

        if (currentIndex >= 0 && navLinks.length > 0) {
            event.preventDefault();
            const direction = event.key === "ArrowDown" ? 1 : -1;
            navLinks[(currentIndex + direction + navLinks.length) % navLinks.length].focus();
        }
        return;
    }

    if (currentLink && event.key === "ArrowRight") {
        event.preventDefault();
        const content = document.querySelector(".content");
        const firstControl = content && getVisibleControls(content)[0];

        if (firstControl) {
            firstControl.focus();
        } else {
            document.querySelector("#main-content")?.focus();
        }
        return;
    }

    if (!target.closest(".content") && !target.matches("#main-content")) {
        return;
    }

    if (target.closest("input, textarea, select, [contenteditable='true'], [role='textbox'], [role='slider']")) {
        return;
    }

    if (!["ArrowDown", "ArrowLeft", "ArrowRight", "ArrowUp"].includes(event.key)) {
        return;
    }

    const content = target.closest(".content");
    const currentControl = content && target.closest(pageControlSelector);
    const controls = content ? getVisibleControls(content) : [];
    const nextControl = currentControl && findDirectionalControl(currentControl, controls, event.key);

    if (nextControl) {
        event.preventDefault();
        nextControl.focus();
        return;
    }

    if (event.key === "ArrowLeft") {
        const activeNavLink = document.querySelector(".sidebar nav a.nav-link.active");
        if (activeNavLink) {
            event.preventDefault();
            activeNavLink.focus();
        }
    }
});
