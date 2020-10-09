export namespace GameMap {
    export class Map {
        private tiles: Tile[];
        private width: number;

        constructor(tiles: Tile[]) {
            this.tiles = tiles;
        }

        GetTile(x: number, y: number) {
            let index = y * this.width;
            index = index + x;
            return this.tiles[index];
        }

        UpdateTile(tile: Tile) {
            let index = tile.y * this.width;
            index = index + tile.x;
            this.tiles[index] = tile;
        }
    }
    export class Tile {
        x: number;
        y: number;
        texture: string;
        passable: boolean;
        destructable: boolean;

        constructor(x: number, y: number, texture: string) {
            this.x = x;
            this.y = y;
            this.texture = texture;
        }
    }
    export class Wall extends Tile {
        constructor(x: number, y: number, texture: string) {
            super(x, y, texture);
            this.passable = false;
            this.destructable = false;
        }
    }
    export class Box extends Tile {
        constructor(x: number, y: number, texture: string) {
            super(x, y, texture);
            this.passable = false;
            this.destructable = true;
        }
    }
    export class EmptyTile extends Tile {
        constructor(x: number, y: number, texture: string) {
            super(x, y, texture);
            this.passable = true;
            this.destructable = false;
        }
    }
}