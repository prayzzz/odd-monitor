import { Category } from "../enums/category"
import { Status } from "../enums/status"
import Str from "../prototype/string";

export class VpGameMatchUrlBuilder {
    private readonly baseUrl = "/api/v1/vpgame/match";
    private readonly pagePattern = "page={0}";
    private readonly categoryPattern = "category={0}";
    private readonly statusPattern = "status={0}";
    private readonly limitPattern = "limit={0}";

    private page = 1;
    private category = "";
    private status = "open";
    private limit = 100;

    public withPage(page: number): VpGameMatchUrlBuilder {
        this.page = page;
        return this;
    }

    public withCategory(category: Category): VpGameMatchUrlBuilder {
        this.category = VpGameCategory.getVpCategory(category);
        return this;
    }

    public withStatus(status: Status): VpGameMatchUrlBuilder {
        this.status = VpGameStatus.getStatus(status);
        return this;
    }

    public withLimit(limit: number): VpGameMatchUrlBuilder {
        this.limit = limit;
        return this;
    }

    public build(): string {
        return this.baseUrl + "?" +
            Str.replace(this.pagePattern, this.page) + "&" +
            Str.replace(this.categoryPattern, this.category) + "&" +
            Str.replace(this.statusPattern, this.status) + "&" +
            Str.replace(this.limitPattern, this.limit);
    }
}

export class VpGameLogoUrlBuilder {
    private readonly base = "http://thumb.vpgcdn.com/crop/91x55/";

    public build(id: string): string {
        return this.base + id;
    }
}

export class VpGameCategory {
    public static getVpCategory(category: Category): string {
        switch (category) {
            case Category.None:
                return "";
            case Category.Basketball:
                return "basketball";
            case Category.Csgo:
                return "Csgo";
            case Category.Dota2:
                return "dota";
            case Category.Soccer:
                return "football";
            case Category.Tennis:
                return "tennis";
            default:
                throw `Unsupported category ${category}`;
        }
    }

    public static getCategory(vpCategory: string): Category {
        switch (vpCategory) {
            case "":
                return Category.None;
            case "basketball":
                return Category.Basketball;
            case "csgo":
                return Category.Csgo;
            case "dota":
                return Category.Dota2;
            case "football":
                return Category.Soccer;
            case "tennis":
                return Category.Tennis;
            default:
                throw `Unsupported category ${vpCategory}`;
        }
    }
}

export class VpGameStatus {
    public static getStatus(status: Status): string {
        switch (status) {
            case Status.Open:
                return "open";
            case Status.Live:
                return "start";
            case Status.Closed:
                return "close";
            default:
                throw "Unsupported staus";
        }
    }
}