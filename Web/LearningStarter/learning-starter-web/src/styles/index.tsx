import { Global } from "@emotion/react";
import { css } from "@emotion/react";
import React from "react";

const globalStyles = css``;

//This is where you put any styles you want to apply to EVERY element
export const GlobalStyles = () => <Global styles={globalStyles} />;
