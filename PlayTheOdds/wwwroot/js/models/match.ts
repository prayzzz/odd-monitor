import Team from "./team"
import Tournament from "./tournament"
import { Category } from "../enums/category"

export default class Match {
    public category: Category;
    public date: Date;
    public id: number;
    public team1: Team;
    public team2: Team;
    public tournament: Tournament;
    public matchLinkBuilder: (m: Match) => string;

    constructor(id: number, date: Date, team1: Team, team2: Team, category: Category, tournament: Tournament) {
        this.category = category;
        this.date = date;
        this.id = id;
        this.team1 = team1;
        this.team2 = team2;
        this.tournament = tournament;

        this.matchLinkBuilder = () => { return "#" };
    }

    public get matchLink(): string {
        return this.matchLinkBuilder(this);
    }
}