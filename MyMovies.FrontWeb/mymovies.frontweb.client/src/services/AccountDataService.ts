import http from "@/httpClient-common";

/* eslint-disable */
class AccountDataService {

    private _token: string = "";
    set token(value: string) {
        this._token = 'Bearer ' + value;
    }

    login(data: any): Promise<any> {
        console.log(data);
        return http.post("/account/login", data);
    }

    register(data: any): Promise<any> {
        console.log(data);
        return http.post("/account/register", data);
    }

    getUserInfo(): Promise<any> {
        console.log();
        return http.get("/account/manage/info", {
            headers: {
                'Authorization': this._token
            }
        });
    }

}

export default new AccountDataService();