import React from 'react';
import { connect } from 'react-redux';
import { withStyles } from '@material-ui/core/styles';
import Button from '@material-ui/core/Button';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';

import { DismissSobrietyPrompt, 
    AffirmativeSobrietyPrompt, 
    NegativeSobrietyPrompt } from '../actions/sobrietyPromptDialog';

const useStyles = theme => ({

});

class SobrietyPromptDialog extends React.Component{

    handleClose = (action) => {
        switch (action) {
            case 'yes':{
                this.props.dispatch(AffirmativeSobrietyPrompt());
                break;
            }
            case 'no':{
                this.props.dispatch(NegativeSobrietyPrompt());
                break;
            }
            default: {
                this.props.dispatch(DismissSobrietyPrompt());
                break;
            }
        }
    }

    render(){
        return (
              <Dialog
                open={this.props.sobrietyPromptDialog.status !== 'closed'}
                onClose={() => this.handleClose('dismiss')}
                aria-labelledby="alert-dialog-title"
                aria-describedby="alert-dialog-description"
              >
                <DialogTitle id="alert-dialog-title">{this.props.sobrietyPromptDialog.title}</DialogTitle>
                <DialogContent>
                  <DialogContentText id="alert-dialog-description">
                    {this.props.sobrietyPromptDialog.question}
                  </DialogContentText>
                </DialogContent>
                <DialogActions>
                  <Button variant="contained" onClick={() => this.handleClose('no')} color="secondary">
                    No
                  </Button>
                  <Button variant="contained" onClick={() => this.handleClose('yes')} color="primary" autoFocus>
                    Yes
                  </Button>
                </DialogActions>
              </Dialog>
        )};
}

SobrietyPromptDialog = connect(
    state => {
      return { 
            sobrietyPromptDialog: state.sobrietyPromptDialog
        };
    }, 
    dispatch => {
        return { dispatch }
    }
  )(SobrietyPromptDialog)

export default (withStyles(useStyles))( SobrietyPromptDialog);