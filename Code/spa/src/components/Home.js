import React from "react";
import { connect } from "react-redux";

class HomePage extends React.Component {
    render(){
      if( !this.props.user ){
        return (
          <div>Welcome, please Login to begin.</div>
        );
      }
      else{
        return (
        <div>Welcome { this.props.userName }, you are logged in.</div>
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

export default connect(mapStateToProps, mapDispatchToProps)(HomePage);