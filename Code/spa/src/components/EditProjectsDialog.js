import React from 'react';
import { connect } from 'react-redux';
import { withStyles } from '@material-ui/core/styles';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button } from '@material-ui/core'
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
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
                fullWidth={true} 
                maxWidth="sm" 
            >
                <DialogTitle id="form-dialog-title">Edit Projects</DialogTitle>
                <DialogContent>
                    <TableContainer>
                        <Table>
                            <TableHead>
                                <TableRow>
                                    <TableCell>Project Name</TableCell>
                                    <TableCell align="right"></TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                            {this.props.projects.map(project => (
                            <TableRow key={project.id}>
                                <TableCell component="th" scope="row">
                                    {project.name}
                                </TableCell>
                                <TableCell align="right">
                                    <Button variant="contained" color="primary">
                                        Edit
                                    </Button>
                                    <span width="200">&nbsp;&nbsp;&nbsp;</span>
                                    <Button variant="contained" color="secondary">
                                        Delete
                                    </Button>
                                </TableCell>
                            </TableRow>
                            ))}
                            </TableBody>
                        </Table>
                    </TableContainer>
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
        var projectData = [];
        if( state.projects && state.projects.Data ){
            projectData = Object.values( state.projects.Data );
        }
      return { 
          projects: projectData, 
          editProjectsDialogState: state.editProjectsDialog,
          userProfile: state.userProfile
        };
    }, 
    dispatch => {
        return { dispatch }
    }
  )(EditProjectsDialog)

export default (withStyles(useStyles))( withSnackbar(EditProjectsDialog));