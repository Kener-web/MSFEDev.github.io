# Vendored SQLite Wasm

The `index.mjs` and `sqlite3.wasm` files are from `@sqlite.org/sqlite-wasm` version `3.54.1-build1`.
The npm package is licensed under Apache 2.0; see `LICENSE`. Its upstream README is available at
<https://github.com/sqlite/sqlite-wasm>; the SQLite and Emscripten license notices are preserved in
`index.mjs`.

NationBuilder loads this package in a dedicated worker and uses the `opfs-sahpool` VFS for browser-local persistence.
