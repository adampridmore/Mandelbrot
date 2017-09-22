//db.tiles.find()

toCsv(db.tiles.aggregate({
  $group:{
      _id: {
          TileSetName : "$TileSetName",
          Zoom : "$Zoom",
      },
      count: {$sum : 1}
  }
},{
  $sort:{
      _id: 1
  }
}))