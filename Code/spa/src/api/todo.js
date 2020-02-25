import axios from "axios";

const createClient = (token) => {
    return axios.create({
        baseURL: 'http://localhost:5001',
        timeout: 5000,
        headers: {'Authorization': 'Bearer '+ token }
    });
}

export const GetIdentity = async (token) => {
    console.log("GetIdentity called.")
    const client = createClient(token);
    console.log("client created.")
    if( !client ){
        return {success: false, errorMessage:"No Token.", data: null}
    }

    try{
        const response = await client.get('/identity');
        console.log(response);
        return { success:true, errorMessage:null, data: response.data }
    }
    catch(err){
        console.log(err);
        return { success:false, errorMessage:err.errorMessage, data: null };
    }
};