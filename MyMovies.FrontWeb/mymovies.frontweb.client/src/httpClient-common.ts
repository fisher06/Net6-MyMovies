import axios from "axios";
import type { AxiosInstance } from "axios";

const apiClient: AxiosInstance = axios.create({
    baseURL: "http://localhost:5094/api",
    headers: {
        "Content-type": "application/json",
    },
});

export default apiClient;