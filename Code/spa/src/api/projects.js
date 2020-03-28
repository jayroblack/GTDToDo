import { createClient, handleError } from './apiCommon';

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
        return handleError(err, "Project");
    }
}

export const UpdateProject = async (token, data) => {
    const client = createClient(token);

    if( !client ){
        return { success:false, errorMessage: "To Token", data:null }
    }

    try{
        const response = await client.put('/project/' + data.id, data);
        return { success:true, errorMessage:null, data: response.data }
    }
    catch(err){
        return handleError(err, "Project");
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
        return handleError(err, "Project");
    }
}