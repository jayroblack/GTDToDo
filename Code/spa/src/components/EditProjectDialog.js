import React from 'react';
import { connect } from 'react-redux';
import { withStyles } from '@material-ui/core/styles';
import { CloseNameFormDialog, SavingNameFormDialog, SaveNameFormDialog } from '../actions/nameFormDialog';
import NameFormDialog from './NameFormDialog'
import { UpdateProject } from '../api/projects';
import { EDIT_PROJECT } from '../actions/types';
const useStyles = theme => ({

});

class EditProjectDialog extends React.Component {

    onSubmit = (formValues) => {
        this.props.dispatch(SavingNameFormDialog());
        this.timer = setTimeout( async () => {
            const data = { id:this.props.nameFormDialog.id, name: formValues.name, versionNumber: this.props.nameFormDialog.versionNumber };

            const response = await UpdateProject(this.props.userProfile.access_token, data);

            var objectToDispatch = 
            {
                type: EDIT_PROJECT,
                payload: response.data
            };
            
            this.props.dispatch(SaveNameFormDialog(response, objectToDispatch));
        } , 2000)
    };

    componentDidUpdateCallback = (prevProps, currentProps) => {
        if( prevProps.nameFormDialog.status === 'saving' && 
        currentProps.nameFormDialog.status === 'saveSucceeded' && 
        prevProps.nameFormDialog.isNew === false ){
            currentProps.enqueueSnackbar('Project Updated.', { key: "ProjectUpdated", persist: false, variant: 'success' });
            currentProps.dispatch(CloseNameFormDialog(true));
        }
    }

    isOpen = () => {
        return this.props.nameFormDialog.status !== 'closed' && !this.props.nameFormDialog.isNew;
    }

    render() {
        
        return (
            <NameFormDialog 
                title="Edit Project" 
                entity="Project" 
                description="Project names must be unique." 
                onSubmit={ this.onSubmit } 
                componentDidUpdateCallback={ this.componentDidUpdateCallback } 
                isOpen={ this.isOpen() }
            />
        );
    }
}

EditProjectDialog = connect(
    state => {
      return { 
          nameFormDialog: state.nameFormDialog,
          userProfile: state.userProfile
        };
    }, 
    dispatch => {
        return { dispatch }
    }
  )(EditProjectDialog)

export default (withStyles(useStyles))( EditProjectDialog);