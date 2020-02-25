import React from "react";
import { connect } from "react-redux";

class ToDo extends React.Component {
    render(){
      return (
        <div> This is the TO DO Component - You cannot get here unless you are logged in!!!!</div>
      );
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

export default connect(mapStateToProps, mapDispatchToProps)(ToDo);