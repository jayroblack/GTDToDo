import React from "react";
import { connect } from "react-redux";
import { GetIdentity } from '../api/todo';

class ToDo extends React.Component {
    async componentDidMount(){
     
    }

    render(){
      console.log("Render");
      if( this.props.user ){
        const { access_token } = this.props.user;
        GetIdentity(access_token)
          .then(data => console.log(data) )
          .catch(err=> console.log(err));
      }
      
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