import axios from "axios";

export const createClient = (token) => {
    return axios.create({
        baseURL: 'http://localhost:5001',
        timeout: 10000,
        headers: {'Authorization': 'Bearer '+ token }
    });
}

export const handleError = (err, entity) => {
    switch(err.response.status){
        case 422: 
            return { success:false, errorMessage: `${entity} Name already exists, try another.`, data: null };
        case 404:
            return { success:false, errorMessage: `Could not find ${entity}.  Try refreshing your screen.`, data: null };
        case 401:
            return { success:false, errorMessage: `The ${entity} you are attempting to modify is denying the request.  Try refreshing your screen.`, data: null };
        case 409:
            return { success:false, errorMessage: `The ${entity} you are attempting to modify is stale.  Try refreshing your screen.`, data: null };
        default:
            return { success:false, errorMessage: err.message, data: null };
    }
}
