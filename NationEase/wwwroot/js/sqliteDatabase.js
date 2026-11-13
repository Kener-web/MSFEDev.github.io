let worker;
let nextRequestId = 0;
const pendingRequests = new Map();
let startup;

function startWorker() {
    if (startup) {
        return startup;
    }

    worker = new Worker(new URL("./sqliteDatabase.worker.js", import.meta.url), { type: "module" });
    startup = new Promise((resolve, reject) => {
        const failAll = (error) => {
            reject(error);
            for (const request of pendingRequests.values()) {
                request.reject(error);
            }
            pendingRequests.clear();
            worker = undefined;
            startup = undefined;
        };

        worker.addEventListener("message", (event) => {
            const message = event.data;
            if (message.type === "ready") {
                resolve();
                return;
            }

            if (message.type === "startup-error") {
                failAll(new Error(message.error));
                return;
            }

            const request = pendingRequests.get(message.id);
            if (!request) {
                return;
            }

            pendingRequests.delete(message.id);
            if (message.error) {
                request.reject(new Error(message.error));
            } else {
                request.resolve(message.result);
            }
        });

        worker.addEventListener("error", (event) => {
            failAll(new Error(event.message || "The SQLite worker failed."));
        });
    });

    return startup;
}

function send(action, args = {}) {
    return startWorker().then(() => new Promise((resolve, reject) => {
        const id = ++nextRequestId;
        pendingRequests.set(id, { resolve, reject });
        worker.postMessage({ id, action, args });
    }));
}

export async function initialize() {
    await startWorker();
}

export function query(sql, parameters) {
    return send("query", { sql, parameters });
}

export function execute(sql, parameters) {
    return send("execute", { sql, parameters });
}

export function createUser(email, password, stateRows, cityRows) {
    return send("createUser", { email, password, stateRows, cityRows });
}

export function seedDefaultCities(userId, cityRows) {
    return send("seedDefaultCities", { userId, cityRows });
}

export function authenticate(email, password) {
    return send("authenticate", { email, password });
}

export function updateUser(userId, email, password) {
    return send("updateUser", { userId, email, password });
}

export function invoke(operation, args) {
    switch (operation) {
        case "createUser":
            return createUser(args[0], args[1], args[2], args[3]);
        case "seedDefaultCities":
            return seedDefaultCities(args[0], args[1]);
        case "authenticate":
            return authenticate(args[0], args[1]);
        case "updateUser":
            return updateUser(args[0], args[1], args[2]);
        default:
            throw new Error(`Unknown SQLite operation: ${operation}`);
    }
}

export function dispose() {
    worker?.terminate();
    worker = undefined;
    startup = undefined;
    for (const request of pendingRequests.values()) {
        request.reject(new Error("The SQLite worker was stopped."));
    }
    pendingRequests.clear();
}
