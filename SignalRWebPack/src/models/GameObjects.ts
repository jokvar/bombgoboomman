export namespace GameObjects {
    class GameObject {
        texture: string;
        x: number;
        y: number;
        textures: Array<String>;
        constructor(x: number, y: number, texture: string, textures: Array<String>) {
            this.x = x;
            this.y = y;
            this.texture = texture;
            this.textures = textures;
        }
    }
    export class Bomb extends GameObject {
        tickDuration: number;
        plantedAt: Date;
        preExplodeTexture: string;
        constructor(x: number, y: number, texture: string, tickDuration: number, plantedAt: Date, preExplodeTexture: string, textures: Array<String>) {
            super(x, y, texture, textures);
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
        constructor(x: number, y: number, texture: string, damage: number, size: number, explosionDuration: number, explodedAt: Date, textures: Array<String>) {
            super(x, y, texture, textures);
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
        constructor(x: number, y: number, texture: string, type: Powerup_type, existDuration: number, plantedAt: Date, textures: Array<String>) {
            super(x, y, texture, textures);
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

