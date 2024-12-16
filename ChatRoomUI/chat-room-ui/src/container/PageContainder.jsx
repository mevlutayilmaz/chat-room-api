import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';

const PageContainer = ({ children }) => {
    return (
        <div className="container min-vh-100 d-flex justify-content-center align-items-center py-4">
            <div className="col-10">{children}</div>
        </div>
    );
};

export default PageContainer;
