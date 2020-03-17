import React from 'react';
import { connect } from 'react-redux';
import { withStyles } from '@material-ui/core/styles';
import { Dialog, DialogTitle, DialogContent, DialogContentText, DialogActions, Button } from '@material-ui/core'
import { CloseEditProjectsDialog } from '../actions/editProjectsDialog';
import { withSnackbar } from 'notistack';

const useStyles = theme => ({

});

class EditProjectsDialog extends React.Component{

    handleDismiss = (cancelled) => {
        this.props.dispatch(CloseEditProjectsDialog(cancelled));
    };

    render() {
                
        return (
            <Dialog open={this.props.editProjectsDialogState.status !== 'closed'} 
            onClose={() => this.handleDismiss(false)} 
            aria-labelledby="form-dialog-title" 
            fullWidth={true} 
            maxWidth="md" 
            open={this.props.editProjectsDialogState.status !== 'closed'}
            >
                <DialogTitle id="form-dialog-title">Edit Projects</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        Add, Edit or Delete Projects.
                    </DialogContentText>

                </DialogContent>
                <DialogActions>
                    <Button variant="contained" onClick={ () => this.handleDismiss(true)} color="primary">
                        Close
                    </Button>
                </DialogActions>
            </Dialog>
        );
    }
}

EditProjectsDialog = connect(
    state => {
      return { 
          projects: state.projects, 
          editProjectsDialogState: state.editProjectsDialog,
          userProfile: state.userProfile
        };
    }, 
    dispatch => {
        return { dispatch }
    }
  )(EditProjectsDialog)

export default (withStyles(useStyles))( withSnackbar(EditProjectsDialog));