export namespace GameObjects {
    class GameObject {
        texture: string;
        x: number;
        y: number;
        constructor(x: number, y: number, texture: string) {
            this.x = x;
            this.y = y;
            this.texture = texture;
        }
    }
    export class Bomb extends GameObject {
        tickDuration: number;
        plantedAt: Date;
        preExplodeTexture: string;
        constructor(x: number, y: number, texture: string, tickDuration: number, plantedAt: Date, preExplodeTexture: string) {
            super(x, y, texture);
            this.tickDuration = tickDuration;
            this.plantedAt = plantedAt;
            this.preExplodeTexture = preExplodeTexture;
        }
    }
    export class Explosion extends GameObject {
        damage: number;
        size: number;
        explosionDuration: number;
        explodedAt: Date;
        constructor(x: number, y: number, texture: string, damage: number, size: number, explosionDuration: number, explodedAt: Date) {
            super(x, y, texture);
            this.damage = damage;
            this.size = size;
            this.explosionDuration = explosionDuration;
            this.explodedAt = explodedAt;
        }
    }
    export class Powerup extends GameObject {
        type: Powerup_type;
        existDuration: number;
        plantedAt: Date;
        constructor(x: number, y: number, texture: string, type: Powerup_type, existDuration: number, plantedAt: Date) {
            super(x, y, texture);
            this.type = type;
            this.existDuration = existDuration;
            this.plantedAt = plantedAt;
        }
    }
    export enum Powerup_type {
        BombDamage,
        BombTickDuration,
        PlayerSpeed,
        ExplosionDamage,
        ExplosionSize,
        AdditionalBomb
    }
}

