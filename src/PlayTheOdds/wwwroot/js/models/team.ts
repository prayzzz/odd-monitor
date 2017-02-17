export default class Team {
    id: number;    
    logoUrl: string;
    name: string;

    constructor(id: number, name: string) {
        this.id = id;
        this.name = name;
    }
}