export namespace Player {
    export class Player {
        lives: number;
        texture: string;
        invulnerable: boolean;
        x: number;
        y: number;
        constructor(lives: number, texture: string, invulnerable: boolean, x: number, y: number) {
            this.lives = lives;
            this.texture = texture;
            this.invulnerable = invulnerable;
            this.x = x;
            this.y = y;
        }
    }
    export class PlayerAction {
        action: ActionEnum;
        constructor(action: ActionEnum) {
            this.action = action;
        }
        
    }
    export enum ActionEnum {
        Up,
        Down,
        Left,
        Right,
        PlaceBomb
    }
}