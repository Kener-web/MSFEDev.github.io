# NationBuilder

NationBuilder is a Blazor WebAssembly learning app. It uses SQLite compiled to WebAssembly and stores its database in the browser's Origin Private File System (OPFS). There is no API server, Docker service, or shared database.

## Run locally

Prerequisites: .NET 9 SDK and a modern browser with OPFS support. OPFS is available on secure origins, including `https://` and `http://localhost`.

```powershell
dotnet run --project NationEase.csproj
```

Open the URL printed by the .NET development server. On first visit, the app initializes its SQLite database in a worker. SQLite's `opfs-sahpool` VFS is used so the app does not require the COOP/COEP headers that static hosts such as GitHub Pages cannot configure.

## Learning features

- Create, sign in to, edit, and delete browser-local user accounts.
- Manage the full US state catalog in ratification/admission order, and create, update, and delete states and cities for each account.
- Select cities for a user's Nation Builder.
- Each account starts with the 50 US states in ratification/admission order (IDs 1-50) and one largest city per state. City IDs 1-50 are ordered by descending 2025 population estimate (New York = 1, Los Angeles = 2); additional cities receive the next account-local ID.
- City seed names and populations use the U.S. Census Bureau's [Vintage 2025 subcounty population estimates](https://www2.census.gov/programs-surveys/popest/datasets/2020-2025/cities/totals/sub-est2025.csv), the latest completed vintage.

Data belongs to the current browser profile and site origin. It is not synchronized to other devices or browsers, and clearing site data can remove it. Private browsing, browser storage restrictions, or an unsupported browser may prevent persistent storage.
The database migration makes city IDs account-local and preserves existing accounts, cities, and selections. Existing cities receive IDs above the 50 seeded cities.

## Security notice

This project is for learning and evaluation only. Accounts and data are stored on the client and are not protected by a trusted server. Passwords are hashed locally, but that does not make these accounts secure. **Do not enter sensitive or real-world credentials or personal information.**

## SQLite WASM

The app vendors the browser runtime from `@sqlite.org/sqlite-wasm` version `3.54.1-build1` under `wwwroot/lib/sqlite-wasm`. Its package README, license information, and upstream usage notes are included alongside the runtime.
