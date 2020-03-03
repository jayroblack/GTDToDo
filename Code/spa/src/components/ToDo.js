import React from "react";
import { connect } from "react-redux";
import { GetorCreateUser, LoadLabelsAndProjectsForUser  } from '../actions';

class ToDo extends React.Component {

    async componentDidMount(){
      
     if( this.props.user ){
        //This is ugly as shit!
        //TODO:  Why is there no default project created?
        const { access_token } = this.props.user;
        const { given_name, family_name, email, sub } = this.props.user.profile;
        const data = { id: sub, firstName: given_name, lastName: family_name, email };
        
        this.props.dispatch(GetorCreateUser(access_token, data));
        this.props.dispatch(LoadLabelsAndProjectsForUser(access_token));
      }
    }

    render(){
      //https://github.com/mui-org/material-ui/issues/11749
      return (
        <div> This is the TO DO Component - You cannot get here unless you are logged in!!!!</div>
      );
    }
}

function mapStateToProps(state) {
    const user = state.oidc.user;
    if( !user ){
      return { user: null };
    }
    return { user: user };
  }

function mapDispatchToProps(dispatch) {
    return {
      dispatch
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(ToDo);