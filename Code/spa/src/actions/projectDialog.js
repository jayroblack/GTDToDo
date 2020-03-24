import { PROJECT_DIALOG_OPEN, 
    PROJECT_DIALOG_CLOSE, 
    PROJECT_DIALOG_SAVING, 
    PROJECT_DIALOG_SAVE_SUCCEEDED, 
    PROJECT_DIALOG_SAVE_FAILED, 
    ADD_PROJECT } from './types';
import { CreateProject, UpdateProject } from '../api/projects';
import { FORM_PROJECT_DIALOG } from '../forms'

export const OpenProjectDialog = (id, name, versionNumber) => {

    return async (dispatch) => {
        dispatch({
            type: "@@redux-form/INITIALIZE",
            meta: { form: FORM_PROJECT_DIALOG },
            payload: {
                name: !name ? '' : name
            }
        });

        dispatch({
            type: PROJECT_DIALOG_OPEN, 
            payload: {
                status: 'open',
                isNew: id ? false : true,
                errorMessage: null,
                cancelled: false,
                data: null,
                id: id,
                versionNumber: versionNumber
            }
        });
    }
};

export const CloseProjectDialog = (cancelled) => {
    return {
        type: PROJECT_DIALOG_CLOSE, 
        payload: {
            status: 'closed',
            isNew: null, 
            errorMessage: null, 
            cancelled: cancelled,
            data: null,
            id: null,
            versionNumber: null
        }
    }
};

export const SavingProjectDialog = () => {
    return {
        type: PROJECT_DIALOG_SAVING, 
        payload: {
            status: 'saving',
            errorMessage: null,
            data: null
        }
    }
};

export const UpdateNewProjectDialog = (token, data) => {
    return async (dispatch) => {
        const response = await UpdateProject(token, data);

        if( response.success ){
            dispatch({
                type: ADD_PROJECT,
                payload: response.data
            });
            dispatch(
            {
                type: PROJECT_DIALOG_SAVE_SUCCEEDED, 
                payload: {
                    status: 'saveSucceeded',
                    errorMessage: null,
                    cancelled: false,
                    data: response.data
                }
            });
        }
        else{
            dispatch(
                {
                    type: PROJECT_DIALOG_SAVE_FAILED, 
                    payload: {
                        status: 'saveFailed',
                        errorMessage: response.errorMessage,
                        cancelled: false,
                        data: null
                    }
                });
        }
    }
}

export const SaveNewProjectDialog = (token, data) => {

    return async (dispatch) => {
        const response = await CreateProject(token, data);
        
        if( response.success){
            dispatch({
                type: ADD_PROJECT,
                payload: response.data
            });
            dispatch(
            {
                type: PROJECT_DIALOG_SAVE_SUCCEEDED, 
                payload: {
                    status: 'saveSucceeded',
                    errorMessage: null,
                    cancelled: false,
                    data: response.data
                }
            });
        }
        else{
            dispatch(
            {
                type: PROJECT_DIALOG_SAVE_FAILED, 
                payload: {
                    status: 'saveFailed',
                    errorMessage: response.errorMessage,
                    cancelled: false,
                    data: null
                }
            });
        }
    }
};