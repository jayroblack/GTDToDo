import { NEWPROJECTDIALOG_OPEN, NEWPROJECTDIALOG_CLOSE, 
NEWPROJECTDIALOG_SAVING, NEWPROJECTDIALOG_SAVE_SUCCEEDED, NEWPROJECTDIALOG_SAVE_FAILED } from './types';
import { CreateProject } from '../api/projects';

export const OpenNewProjectDialog = () => {
    return {
        type: NEWPROJECTDIALOG_OPEN, 
        payload: {
            status: 'open',
            errorMessage: null,
            cancelled: false,
            data: null
        }
    }
};

export const CloseNewProjectDialog = (cancelled) => {
    return {
        type: NEWPROJECTDIALOG_CLOSE, 
        payload: {
            status: 'closed',
            errorMessage: null, 
            cancelled: cancelled,
            data: null
        }
    }
};

export const SavingNewProjectDialog = () => {
    return {
        type: NEWPROJECTDIALOG_SAVING, 
        payload: {
            status: 'saving',
            errorMessage: null,
            cancelled: false,
            data: null
        }
    }
};

export const SaveNewProjectDialog = (token, data) => {

    return async (dispatch) => {
        const response = await CreateProject(token, data);

        if( response.success){
            dispatch(
            {
                type: NEWPROJECTDIALOG_SAVE_SUCCEEDED, 
                payload: {
                    status: 'saveSuceeded',
                    errorMessage: null,
                    cancelled: false,
                    data: response.data
                }
            });
        }
        else{
            dispatch(
            {
                type: NEWPROJECTDIALOG_SAVE_FAILED, 
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