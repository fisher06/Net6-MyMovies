import http from "@/httpClient-common";

/* eslint-disable */
class MovieDataService {
    private _token: string = "";
    set token(value: string) {
        this._token = 'Bearer ' + value;
    }

    getAll(): Promise<any> {
        return http.get("/movies");
    }
    get(id: any): Promise<any> {
        return http.get(`/movies/${id}`, {
            headers: { 'Authorization': this._token }
        });
    }
    update(id: any, data: any): Promise<any> {
        return http.put(`/movies/${id}`, data, {
            headers: { 'Authorization': this._token }
        });
    }
    delete(id: any): Promise<any> {
        return http.delete(`/movies/${id}`, {
            headers: { 'Authorization': this._token }
        });
    }
    create(data: any): Promise<any> {
        return http.post("/movies", data, {
            headers: { 'Authorization': this._token }
        });
    }
}
export default new MovieDataService();