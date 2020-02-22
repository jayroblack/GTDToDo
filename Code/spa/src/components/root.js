import React from 'react';

function Root(props) {
  return (
    <React.Fragment>
        { props.children }
    </ React.Fragment>
  );
}

export default Root;