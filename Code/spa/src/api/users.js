import createClient from './todoApiConfig';

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

export const GetOrCreateUser = async (token, data) => {
    const client = createClient(token);
    if( !client ){
        return {success: false, errorMessage:"No Token.", data: null}
    }

    try{
        const response = await client.post('users/GetOrCreateUser', data );
        return { success:true, errorMessage:null, data: response.data }
    }
    catch(err){
        return { success:false, errorMessage:err.errorMessage, data: null };
    }
}

export const GetToDos = async(token) =>{
    const client = createClient(token);
    if( !client ){
        return {success: false, errorMessage:"No Token.", data: null}
    }

    try{
        const response = await client.get('/todo');
        return { success:true, errorMessage:null, data: response.data }
    }
    catch(err){
        return { success:false, errorMessage:err.errorMessage, data: null };
    }
}