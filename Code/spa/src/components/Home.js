import React from "react";
import { connect } from "react-redux";
import { Redirect } from 'react-router-dom'
import { withStyles } from '@material-ui/core/styles';

const useStyles = theme => ({
  toolbar: theme.mixins.toolbar,
});

class HomePage extends React.Component {
    render(){
      const { classes } = this.props;

      if( !this.props.user ){
        return (
          <div>Welcome, please Login to begin.</div>
        );
      }
      else{
        return (
          <Redirect to="/todo" />
        );
      }
    }
}

function mapStateToProps(state) {
    const user = state.oidc.user;
    if( !user ){
      return { user: null, userName: null };
    }
    return { user: user, userName: user.profile.name };
  }

function mapDispatchToProps(dispatch) {
    return {
      dispatch
    };
}

export default connect(mapStateToProps, mapDispatchToProps)((withStyles(useStyles)(HomePage)));