import axios from "axios";

const createClient = (token) => {
    return axios.create({
        baseURL: 'http://localhost:5001',
        timeout: 10000,
        headers: {'Authorization': 'Bearer '+ token }
    });
}

export default createClient;