var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var MapSpace;
(function (MapSpace) {
    var Map = /** @class */ (function () {
        function Map(tiles) {
            this.tiles = tiles;
        }
        Map.prototype.GetTile = function (x, y) {
            var index = y * this.width;
            index = index + x;
            return this.tiles[index];
        };
        Map.prototype.UpdateTile = function (tile) {
            var index = tile.y * this.width;
            index = index + tile.x;
            this.tiles[index] = tile;
        };
        return Map;
    }());
    MapSpace.Map = Map;
    var Tile = /** @class */ (function () {
        function Tile(x, y, texture) {
            this.x = x;
            this.y = y;
            this.texture = texture;
        }
        return Tile;
    }());
    MapSpace.Tile = Tile;
    var Wall = /** @class */ (function (_super) {
        __extends(Wall, _super);
        function Wall(x, y, texture) {
            var _this = _super.call(this, x, y, texture) || this;
            _this.passable = false;
            _this.destructable = false;
            return _this;
        }
        return Wall;
    }(Tile));
    MapSpace.Wall = Wall;
    var Box = /** @class */ (function (_super) {
        __extends(Box, _super);
        function Box(x, y, texture) {
            var _this = _super.call(this, x, y, texture) || this;
            _this.passable = false;
            _this.destructable = true;
            return _this;
        }
        return Box;
    }(Tile));
    MapSpace.Box = Box;
})(MapSpace || (MapSpace = {}));
//# sourceMappingURL=gameBackend.js.map