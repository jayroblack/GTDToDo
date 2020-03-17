import { EDIT_PROJECTS_DIALOG_OPEN, 
    EDIT_PROJECTS_DIALOG_CLOSE, 
     } from './types';

export const OpenEditProjectsDialog = () => {
    return {
        type: EDIT_PROJECTS_DIALOG_OPEN, 
        payload: { status: 'open' }
    }
};

export const CloseEditProjectsDialog = (cancelled) => {
    return {
        type: EDIT_PROJECTS_DIALOG_CLOSE, 
        payload: { status: 'closed' }
    }
};