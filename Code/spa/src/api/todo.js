import axios from "axios";

const createClient = (token) => {
    return axios.create({
        baseURL: 'http://localhost:5001',
        timeout: 5000,
        headers: {'Authorization': 'Bearer '+ token }
    });
}

export const GetIdentity = async (token) => {
    const client = createClient(token);
    
    if( !client ){
        return {success: false, errorMessage:"No Token.", data: null}
    }

    try{
        const response = await client.get('/identity');
        return { success:true, errorMessage:null, data: response.data }
    }
    catch(err){
        return { success:false, errorMessage:err.errorMessage, data: null };
    }
};