import createClient from './todoApiConfig';

export const CreateProject = async (token, data) => {
    const client = createClient(token);
    
    if( !client ){
        return {success: false, errorMessage:"No Token.", data: null}
    }

    try{
        const response = await client.post('/project', data);
        return { success:true, errorMessage:null, data: response.data }
    }
    catch(err){
        if( err.message === 'Request failed with status code 422' ){
            return { success:false, errorMessage: "Project Name already exists, try another.", data: null };
        }
        else{
            return { success:false, errorMessage:err.errorMessage, data: null, err: err };
        }
    }
}

export const DeleteProject = async (token, projectId) => {
    const client = createClient(token);
    
    if( !client ){
        return {success: false, errorMessage:"No Token.", data: null}
    }

    try{
        const response = await client.delete('/project/' + projectId);
        return { success:true, errorMessage:null, data: response.data }
    }
    catch(err){
        return { success:false, errorMessage:err.errorMessage, data: null, err: err };
    }
}