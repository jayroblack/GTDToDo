import { NEWPROJECTDIALOG_OPEN, NEWPROJECTDIALOG_CLOSE, 
NEWPROJECTDIALOG_INVALID, NEWPROJECTDIALOG_SAVE,
NEWPROJECTDIALOG_SAVE_SUCCEEDED, NEWPROJECTDIALOG_SAVE_FAILED } from './types';

export const OpenNewProjectDialog = () => {
    return {
        type: NEWPROJECTDIALOG_OPEN, 
        payload: {
            status: 'open',
            errorMessage: null,
            cancelled: false
        }
    }
};

export const CloseNewProjectDialog = (cancelled) => {
    return {
        type: NEWPROJECTDIALOG_CLOSE, 
        payload: {
            status: 'closed',
            errorMessage: null, 
            cancelled: cancelled
        }
    }
};

export const InvalidateNewProjectDialog = (errorMessage) => {
    return {
        type: NEWPROJECTDIALOG_INVALID, 
        payload: {
            status: 'invalid',
            errorMessage: errorMessage,
            cancelled: false
        }
    }
};

export const SaveNewProjectDialog = (token, data) => {
    return {
        type: NEWPROJECTDIALOG_SAVE, 
        payload: {
            status: 'saving',
            errorMessage: null,
            cancelled: false
        }
    }
};

export const SaveNewProjectDialogSucceeded = () => {
    return {
        type: NEWPROJECTDIALOG_SAVE_SUCCEEDED, 
        payload: {
            status: 'saveSuceeded',
            errorMessage: null,
            cancelled: false
        }
    }
};

export const SaveNewProjectDialogFailed = (errorMessage) => {
    return {
        type: NEWPROJECTDIALOG_SAVE_FAILED, 
        payload: {
            status: 'saveFailed',
            errorMessage: errorMessage,
            cancelled: false
        }
    }
};