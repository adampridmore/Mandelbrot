db.tiles.createIndex({
    TileSetName: 1,
    Zoom: 1,
    X: 1,
    Y: 1,     
}, { unique: true });
