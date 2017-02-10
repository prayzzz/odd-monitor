export default class Str {
    public static replace(str: string, ...args: any[]): string {
        return str.replace(/{(\d+)}/g, (match, number) => typeof args[number] != "undefined" ? args[number] : match);
    };
}
