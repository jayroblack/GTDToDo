import React from "react";
import { connect } from "react-redux";
import { GetOrCreateUser } from '../api/todo';

class ToDo extends React.Component {

    async componentDidMount(){

     if( this.props.user ){
        //In the event that this is the first time the user has visited, create the user. 
        const { access_token } = this.props.user;
        const { given_name, family_name, email, sub } = this.props.user.profile;
        const data = { id: sub, firstName: given_name, lastName: family_name, email };
        
        GetOrCreateUser(access_token, data)
          .then(data => console.log(data) )
          .catch(err=> console.log(err));
      }
    }

    render(){
      
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