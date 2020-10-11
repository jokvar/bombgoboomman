export namespace GameMap {
    export class Map {
        private tiles: Tile[];
        private width: number;
        private mapData: number[] = [1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1,
            0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0,
            1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1];
        private index: number;

        constructor() {
            this.index = 0;
            this.mapData.forEach(function (value) {
                var x = this.index % 15;
                var y = this.index / 15;

                if (value == 0) {
                    let tile = new EmptyTile(x, y, "test");
                    this.tiles[this.index] = tile;
                }
                if (value == 1) {
                    let tile = new Wall(x, y, "test");
                    this.tiles[this.index] = tile;
                }
                this.index++;
            });
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