import createClient from './todoApiConfig';

export const CreateProject = async (token, projectName) => {
    const client = createClient(token);
    
    if( !client ){
        return {success: false, errorMessage:"No Token.", data: null}
    }

    try{
        const response = await client.post('/project', projectName);
        return { success:true, errorMessage:null, data: response.data }
    }
    catch(err){
        return { success:false, errorMessage:err.errorMessage, data: null };
    }
}