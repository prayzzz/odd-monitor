interface IAjaxRequest<T> {
    success(cb: (data: T) => void): IAjaxRequest<T>;
    error(cb: (data: T) => void): IAjaxRequest<T>;
    always(cb: (data: T) => void): IAjaxRequest<T>;
    send(): void;
}

export default class Ajax {
    public static get<T>(url: string): IAjaxRequest<T> {
        return new AjaxRequest<T>("GET", url);
    }
}

class AjaxRequest<T> implements IAjaxRequest<T> {
    private method: string;
    private url: string;

    private successCb: (data: T) => void;
    private errorCb: (data: T) => void;
    private alwaysCb: (data: T) => void;

    constructor(method: string, url: string) {
        this.method = method;
        this.url = url;

        this.successCb = d => { };
        this.errorCb = d => { };
        this.alwaysCb = d => { };
    }

    public success(cb: (data: T) => void): IAjaxRequest<T> {
        this.successCb = cb;
        return this;
    }

    public error(cb: (data: T) => void): IAjaxRequest<T> {
        this.errorCb = cb;
        return this;
    }

    public always(cb: (data: T) => void): IAjaxRequest<T> {
        this.alwaysCb = cb;
        return this;
    }

    public send() {
        const request = new XMLHttpRequest();
        request.open(this.method, this.url, true);
        request.setRequestHeader("Content-Type", "application/json");
        request.onreadystatechange = (event: Event) => {
            if (4 !== request.readyState) {
                return;
            }

            this.alwaysCb(this.parse(request))
        }

        request.onerror = (e: ErrorEvent) => {
            this.errorCb(this.parse(request))
        }

        request.onloadend = (e: ProgressEvent) => {
            if (request.status >= 200 && request.status < 300) {
                this.successCb(this.parse(request))
            }
            else {
                this.errorCb(this.parse(request))
            }
        }

        request.send()
    }

    private parse(request: XMLHttpRequest): T {
        var result: T;
        try {
            result = JSON.parse(request.responseText);
        } catch (e) {
            throw e;
        }
        return result;
    };
}