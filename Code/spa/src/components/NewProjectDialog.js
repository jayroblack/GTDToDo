import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm, formValueSelector  } from 'redux-form';
import { withStyles } from '@material-ui/core/styles';
import { Dialog, DialogTitle, DialogContent, DialogContentText, DialogActions, Button, TextField } from '@material-ui/core'
import { CloseNewProjectDialog, InvalidateNewProjectDialog, SaveNewProjectDialog } from '../actions/newProjectDialog';

const useStyles = theme => ({

});

class NewProjectDialog extends React.Component {
    
    handleDismiss = (cancelled) => {
        this.props.dispatch(CloseNewProjectDialog(cancelled));
    };

    handleSave = () => {
        //How do I get the value of the project? 
        //If length == 0 then 1) Make field Red - show error message 2) Disable the Save Button
        //If Length > 0 however the Project Name already exists 1) Make field Red - show error message 2) Disable the Save Button
        
        //When saving, - change to spinner - save and cancel are disabled
        //When Save Returns Succeesful - Close Dialog - show Snack Bar Success.
        //When Save Returns Failed - 1) Make Field Red - show error message - cancel and save are enabled again.
        //this.props.dispatch(SaveNewProjectDialog());
    };

    renderTextField = ({ input, label, meta: { touched, error }, ...custom }) => (
        <TextField autoFocus
            required 
            label={label}
            margin="dense"
            fullWidth
          {...input}
          {...custom}
        />
    )

    render() {
        return (
            <form name="newProjectDialog">
            <Dialog open={this.props.newProjectDialog.status !== 'closed'} onClose={() => this.handleDismiss(false)} aria-labelledby="form-dialog-title">
                <DialogTitle id="form-dialog-title">Add New Project</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        Enter the new project name and click save.
                    </DialogContentText>
                    <Field name="projectName" label="Project Name" component={this.renderTextField} type="text" />
                </DialogContent>
                <DialogActions>
                    <Button onClick={ ()=> this.handleDismiss(true)} color="primary">
                        Cancel
                    </Button>
                    <Button onClick={this.handleSave} color="primary">
                        Save
                    </Button>
                </DialogActions>
            </Dialog>
            </form>
        );
    }
}

NewProjectDialog = reduxForm({
    form: 'newProjectDialog'
})(NewProjectDialog)

NewProjectDialog = connect(
    state => {
      return { 
          projects: state.labelsAndProjects, 
          newProjectDialog: state.newProjectDialog 
        };
    }, 
    dispatch => {
        return { dispatch }
    }
  )(NewProjectDialog)


export default (withStyles(useStyles))(NewProjectDialog);