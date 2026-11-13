import sqlite3InitModule from "../lib/sqlite-wasm/index.mjs";

const schema = `
    PRAGMA foreign_keys = ON;
    CREATE TABLE IF NOT EXISTS Users (
        UserId INTEGER PRIMARY KEY AUTOINCREMENT,
        Email TEXT NOT NULL COLLATE NOCASE UNIQUE,
        PasswordHash TEXT NOT NULL,
        PasswordSalt TEXT NOT NULL
    );
    CREATE TABLE IF NOT EXISTS States (
        StateId INTEGER NOT NULL,
        OwnerUserId INTEGER NOT NULL,
        Date TEXT NOT NULL,
        Acronym TEXT NOT NULL COLLATE NOCASE,
        Name TEXT NOT NULL,
        PRIMARY KEY (OwnerUserId, StateId),
        UNIQUE (OwnerUserId, Acronym),
        FOREIGN KEY (OwnerUserId) REFERENCES Users (UserId) ON DELETE CASCADE
    );
    CREATE TABLE IF NOT EXISTS Cities (
        CityId INTEGER NOT NULL,
        OwnerUserId INTEGER NOT NULL,
        StateId INTEGER NOT NULL,
        Name TEXT NOT NULL,
        Population REAL NOT NULL,
        FoundingDate TEXT,
        PRIMARY KEY (OwnerUserId, CityId),
        FOREIGN KEY (OwnerUserId) REFERENCES Users (UserId) ON DELETE CASCADE,
        FOREIGN KEY (OwnerUserId, StateId) REFERENCES States (OwnerUserId, StateId) ON DELETE RESTRICT
    );
    CREATE TABLE IF NOT EXISTS CityCatalogSeeds (
        OwnerUserId INTEGER PRIMARY KEY,
        FOREIGN KEY (OwnerUserId) REFERENCES Users (UserId) ON DELETE CASCADE
    );
    CREATE TABLE IF NOT EXISTS UserCities (
        UserId INTEGER NOT NULL,
        CityId INTEGER NOT NULL,
        PRIMARY KEY (UserId, CityId),
        FOREIGN KEY (UserId) REFERENCES Users (UserId) ON DELETE CASCADE,
        FOREIGN KEY (UserId, CityId) REFERENCES Cities (OwnerUserId, CityId) ON DELETE CASCADE
    );
`;

let database;

function normalizeEmail(email) {
    return email.trim().toLowerCase();
}

function toHex(bytes) {
    return Array.from(bytes, (byte) => byte.toString(16).padStart(2, "0")).join("");
}

function fromHex(value) {
    return Uint8Array.from(value.match(/.{2}/g) ?? [], (byte) => Number.parseInt(byte, 16));
}

async function hashPassword(password, salt) {
    const key = await crypto.subtle.importKey(
        "raw",
        new TextEncoder().encode(password),
        "PBKDF2",
        false,
        ["deriveBits"]);
    const hash = await crypto.subtle.deriveBits(
        { name: "PBKDF2", salt, iterations: 210000, hash: "SHA-256" },
        key,
        256);
    return new Uint8Array(hash);
}

async function createPasswordHash(password) {
    const salt = crypto.getRandomValues(new Uint8Array(16));
    return {
        passwordSalt: toHex(salt),
        passwordHash: toHex(await hashPassword(password, salt))
    };
}

function query(sql, parameters = []) {
    const rows = [];
    database.exec({ sql, bind: parameters, rowMode: "object", resultRows: rows });
    return rows;
}

function execute(sql, parameters = []) {
    database.exec({ sql, bind: parameters });
    return database.changes();
}

async function initialize() {
    const sqlite3 = await sqlite3InitModule();
    const pool = await sqlite3.installOpfsSAHPoolVfs({
        name: "nationease",
        directory: "nationease-opfs"
    });
    database = new pool.OpfsSAHPoolDb("/nationease.sqlite3");
    const schemaVersion = query("PRAGMA user_version")[0].user_version;
    if (schemaVersion < 4) {
        database.exec("PRAGMA foreign_keys = OFF");
        database.exec("BEGIN");
        try {
            database.exec(`
                DROP TABLE IF EXISTS UserCities;
                DROP TABLE IF EXISTS Cities;
                DROP TABLE IF EXISTS States;
                DROP TABLE IF EXISTS Users;
            `);
            database.exec(schema);
            database.exec("PRAGMA user_version = 5");
            database.exec("COMMIT");
        } catch (error) {
            database.exec("ROLLBACK");
            throw error;
        } finally {
            database.exec("PRAGMA foreign_keys = ON");
        }
    } else if (schemaVersion < 5) {
        database.exec("PRAGMA foreign_keys = OFF");
        database.exec("BEGIN");
        try {
            database.exec(`
                CREATE TABLE Cities_v5 (
                    CityId INTEGER NOT NULL,
                    OwnerUserId INTEGER NOT NULL,
                    StateId INTEGER NOT NULL,
                    Name TEXT NOT NULL,
                    Population REAL NOT NULL,
                    FoundingDate TEXT,
                    PRIMARY KEY (OwnerUserId, CityId),
                    FOREIGN KEY (OwnerUserId) REFERENCES Users (UserId) ON DELETE CASCADE,
                    FOREIGN KEY (OwnerUserId, StateId) REFERENCES States (OwnerUserId, StateId) ON DELETE RESTRICT
                );
                CREATE TABLE UserCities_v5 (
                    UserId INTEGER NOT NULL,
                    CityId INTEGER NOT NULL,
                    PRIMARY KEY (UserId, CityId),
                    FOREIGN KEY (UserId) REFERENCES Users (UserId) ON DELETE CASCADE,
                    FOREIGN KEY (UserId, CityId) REFERENCES Cities_v5 (OwnerUserId, CityId) ON DELETE CASCADE
                );
                INSERT INTO Cities_v5 (CityId, OwnerUserId, StateId, Name, Population, FoundingDate)
                SELECT CityId + 50, OwnerUserId, StateId, Name, Population, FoundingDate
                FROM Cities;
                INSERT INTO UserCities_v5 (UserId, CityId)
                SELECT UserId, CityId + 50
                FROM UserCities;
                DROP TABLE UserCities;
                DROP TABLE Cities;
                ALTER TABLE Cities_v5 RENAME TO Cities;
                ALTER TABLE UserCities_v5 RENAME TO UserCities;
            `);
            database.exec(schema);
            database.exec("PRAGMA user_version = 5");
            database.exec("COMMIT");
        } catch (error) {
            database.exec("ROLLBACK");
            throw error;
        } finally {
            database.exec("PRAGMA foreign_keys = ON");
        }
    } else {
        database.exec(schema);
    }
}

async function handle(action, args) {
    switch (action) {
        case "query":
            return query(args.sql, args.parameters);
        case "execute":
            return execute(args.sql, args.parameters);
        case "createUser": {
            const email = normalizeEmail(args.email);
            const { passwordHash, passwordSalt } = await createPasswordHash(args.password);
            database.exec("BEGIN");
            try {
                execute(
                    "INSERT INTO Users (Email, PasswordHash, PasswordSalt) VALUES (?, ?, ?)",
                    [email, passwordHash, passwordSalt]);
                const user = query("SELECT UserId, Email FROM Users WHERE Email = ?", [email])[0];
                for (const [stateId, date, acronym, name] of args.stateRows) {
                    execute(
                        "INSERT INTO States (StateId, OwnerUserId, Date, Acronym, Name) VALUES (?, ?, ?, ?, ?)",
                        [stateId, user.UserId, date, acronym, name]);
                }
                for (const [cityId, stateId, name, population] of args.cityRows) {
                    execute(
                        "INSERT INTO Cities (CityId, OwnerUserId, StateId, Name, Population) VALUES (?, ?, ?, ?, ?)",
                        [cityId, user.UserId, stateId, name, population]);
                }
                execute("INSERT INTO CityCatalogSeeds (OwnerUserId) VALUES (?)", [user.UserId]);

                database.exec("COMMIT");
                return user;
            } catch (error) {
                database.exec("ROLLBACK");
                throw error;
            }
        }
        case "seedDefaultCities": {
            if (query("SELECT 1 FROM CityCatalogSeeds WHERE OwnerUserId = ?", [args.userId]).length > 0) {
                return false;
            }

            database.exec("BEGIN");
            try {
                for (const [cityId, stateId, name, population] of args.cityRows) {
                    execute(
                        "INSERT INTO Cities (CityId, OwnerUserId, StateId, Name, Population) VALUES (?, ?, ?, ?, ?)",
                        [cityId, args.userId, stateId, name, population]);
                }
                execute("INSERT INTO CityCatalogSeeds (OwnerUserId) VALUES (?)", [args.userId]);
                database.exec("COMMIT");
                return true;
            } catch (error) {
                database.exec("ROLLBACK");
                throw error;
            }
        }
        case "authenticate": {
            const email = normalizeEmail(args.email);
            const user = query(
                "SELECT UserId, Email, PasswordHash, PasswordSalt FROM Users WHERE Email = ?",
                [email])[0];
            if (!user) {
                return null;
            }

            const actual = await hashPassword(args.password, fromHex(user.PasswordSalt));
            const expected = fromHex(user.PasswordHash);
            let difference = actual.length ^ expected.length;
            for (let index = 0; index < Math.min(actual.length, expected.length); index++) {
                difference |= actual[index] ^ expected[index];
            }

            return difference === 0
                ? { UserId: user.UserId, Email: user.Email }
                : null;
        }
        case "updateUser": {
            const email = normalizeEmail(args.email);
            if (args.password) {
                const { passwordHash, passwordSalt } = await createPasswordHash(args.password);
                execute(
                    "UPDATE Users SET Email = ?, PasswordHash = ?, PasswordSalt = ? WHERE UserId = ?",
                    [email, passwordHash, passwordSalt, args.userId]);
            } else {
                execute("UPDATE Users SET Email = ? WHERE UserId = ?", [email, args.userId]);
            }

            return query("SELECT UserId, Email FROM Users WHERE UserId = ?", [args.userId])[0] ?? null;
        }
        default:
            throw new Error(`Unknown SQLite operation: ${action}`);
    }
}

const initialized = initialize();
initialized.then(
    () => self.postMessage({ type: "ready" }),
    (error) => self.postMessage({ type: "startup-error", error: error.message }));

self.addEventListener("message", async ({ data }) => {
    try {
        await initialized;
        const result = await handle(data.action, data.args);
        self.postMessage({ id: data.id, result });
    } catch (error) {
        self.postMessage({ id: data.id, error: error.message });
    }
});
