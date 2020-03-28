import { NAME_FORM_DIALOG_OPEN, 
    NAME_FORM_DIALOG_CLOSE, 
    NAME_FORM_DIALOG_SAVING, 
    NAME_FORM_DIALOG_SAVE_SUCCEEDED, 
    NAME_FORM_DIALOG_SAVE_FAILED } from './types';
import { NAME_FORM_DIALOG } from '../forms'

export const OpenNameFormDialog = (id, name, versionNumber, entity) => {

    return async (dispatch) => {
        dispatch({
            type: "@@redux-form/INITIALIZE",
            meta: { form: NAME_FORM_DIALOG },
            payload: {
                name: !name ? '' : name
            }
        });

        dispatch({
            type: NAME_FORM_DIALOG_OPEN, 
            payload: {
                status: 'open',
                isNew: id ? false : true,
                errorMessage: null,
                cancelled: false,
                data: null,
                id: id,
                versionNumber: versionNumber,
                entity: entity
            }
        });
    }
};

export const CloseNameFormDialog = (cancelled) => {
    return {
        type: NAME_FORM_DIALOG_CLOSE, 
        payload: {
            status: 'closed',
            isNew: null, 
            errorMessage: null, 
            cancelled: cancelled,
            data: null,
            id: null,
            versionNumber: null,
            entity: null
        }
    }
};

export const SavingNameFormDialog = () => {
    return {
        type: NAME_FORM_DIALOG_SAVING, 
        payload: {
            status: 'saving',
            errorMessage: null,
            data: null
        }
    }
};

export const SaveNameFormDialog = (response, objectToDispatch) => {

    return async (dispatch) => {
        
        if( response.success){

            dispatch(objectToDispatch);

            dispatch(
            {
                type: NAME_FORM_DIALOG_SAVE_SUCCEEDED, 
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
                type: NAME_FORM_DIALOG_SAVE_FAILED, 
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