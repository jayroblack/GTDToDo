import { EDIT_PROJECTS_DIALOG_OPEN, 
    EDIT_PROJECTS_DIALOG_CLOSE, 
    EDIT_PROJECTS_DIALOG_PENDING_RESPONSE,
    EDIT_PROJECTS_DIALOG_RESPONSE_SUCCEEDED,
    EDIT_PROJECTS_DIALOG_RESPONSE_FAILED,
    DELETED_PROJECT
    } from './types';
import { DeleteProject } from '../api/projects';

export const OpenEditProjectsDialog = () => {
    return {
        type: EDIT_PROJECTS_DIALOG_OPEN, 
        payload: { 
            status: 'open',
            cancelled: false,
            errorMessage: null,
            data: null
        }
    }
};

export const CloseEditProjectsDialog = (cancelled) => {
    return {
        type: EDIT_PROJECTS_DIALOG_CLOSE, 
        payload: { 
            status: 'closed', 
            cancelled: cancelled,
            errorMessage: null,
            data: null
        }
    }
};

export const PendingEditProjectsDialog = () => {
    return {
        type: EDIT_PROJECTS_DIALOG_PENDING_RESPONSE, 
        payload: {
            status: 'pending',
            cancelled: false,
            errorMessage: null,
            data: null
        }
    }
};

export const DeleteProjectEditProjectsDialog = (token, projectId) => {

    return async (dispatch) => {
        const response = await DeleteProject(token, projectId);

        if( response.success){

            dispatch({
                type: DELETED_PROJECT,
                payload: projectId
            });

            dispatch({
                type: EDIT_PROJECTS_DIALOG_RESPONSE_SUCCEEDED,
                payload:{
                    status: 'succeeded',
                    cancelled: false,
                    errorMessage: null,
                    data: null
                }
            });

        }
        else{
            dispatch(
            {
                type: EDIT_PROJECTS_DIALOG_RESPONSE_FAILED, 
                payload: {
                    status: 'failed',
                    cancelled: false,
                    errorMessage: response.errorMessage,
                    data: null
                }
            });
        }
    };
};