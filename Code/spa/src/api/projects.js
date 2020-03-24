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
        switch(err.response.status){
            case 422: 
                return { success:false, errorMessage: "Project Name already exists, try another.", data: null };
            case 404:
                return { success:false, errorMessage: "Could not find Project.  Try refreshing your screen.", data: null };
            case 401:
                return { success:false, errorMessage: "The Project you are attempting to modify is denying the request.  Try refreshing your screen.", data: null };
            case 409:
                return { success:false, errorMessage: "The Project you are attempting to modify is stale.  Try refreshing your screen.", data: null };
            default:
                return { success:false, errorMessage: err.message, data: null };
        }
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
        switch(err.response.status){
            case 422: 
                return { success:false, errorMessage: "Project Name already exists, try another.", data: null };
            case 404:
                return { success:false, errorMessage: "Could not find Project.  Try refreshing your screen.", data: null };
            case 401:
                return { success:false, errorMessage: "The Project you are attempting to modify is denying the request.  Try refreshing your screen.", data: null };
            case 409:
                return { success:false, errorMessage: "The Project you are attempting to modify is stale.  Try refreshing your screen.", data: null };
            default:
                return { success:false, errorMessage: err.message, data: null };
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
        switch(err.response.status){
            case 422: 
                return { success:false, errorMessage: "Project Name already exists, try another.", data: null };
            case 404:
                return { success:false, errorMessage: "Could not find Project.  Try refreshing your screen.", data: null };
            case 401:
                return { success:false, errorMessage: "The Project you are attempting to modify is denying the request.  Try refreshing your screen.", data: null };
            case 409:
                return { success:false, errorMessage: "The Project you are attempting to modify is stale.  Try refreshing your screen.", data: null };
            default:
                return { success:false, errorMessage: err.message, data: null };
        }
    }
}