import "./navigation.css";
import React, { useMemo } from "react";
import { Link, NavLink, NavLinkProps } from "react-router-dom";
import { Dropdown, Image, Menu, Icon, SemanticICONS } from "semantic-ui-react";
import logo from "../assets/logo.png";
import { User } from "../types";
import { logoutUser } from "../authentication/authentication-services";
import { routes } from "../routes/config";

type PrimaryNavigationProps = {
  user: User;
};

type NavigationItem = {
  text: string;
  icon?: SemanticICONS | undefined;
  hide?: boolean;
} & (
  | {
      nav: Omit<
        NavLinkProps,
        keyof React.AnchorHTMLAttributes<HTMLAnchorElement>
      >;
      children?: never;
    }
  | { nav?: never; children: NavigationItem[] }
);

const DesktopNavigation = () => {
  const navigation: NavigationItem[] = useMemo(() => {
    return [
      {
        text: "Home",
        icon: "home",
        hide: false,
        nav: {
          to: routes.root,
        },
      },
      {
        text: "User",
        icon: "user",
        hide: false,
        nav: {
          to: routes.user,
        },
      },
    ];
  }, []);

  return (
    <Menu
      secondary
      role="navigation"
      className="desktop-navigation"
      size="large"
    >
      {navigation
        .filter((x) => !x.hide)
        .map((x, i) => {
          if (x.children) {
            return (
              <Dropdown
                key={i}
                trigger={
                  <span>
                    {x.icon && <Icon size="small" fitted name={x.icon} />}{" "}
                    {x.text}
                  </span>
                }
                pointing
                className="link item"
              >
                <Dropdown.Menu>
                  {x.children
                    .filter((x) => !x.hide)
                    .map((y) => {
                      return (
                        <Dropdown.Item
                          key={`${y.text}`}
                          as={NavLink}
                          to={y.nav?.to}
                        >
                          {y.icon && <Icon size="small" fitted name={x.icon} />}{" "}
                          {y.text}
                        </Dropdown.Item>
                      );
                    })}
                </Dropdown.Menu>
              </Dropdown>
            );
          }
          return (
            <Menu.Item key={i} as={NavLink} {...x.nav}>
              {x.icon && <Icon size="small" name={x.icon} />} {x.text}
            </Menu.Item>
          );
        })}
    </Menu>
  );
};

export const PrimaryNavigation: React.FC<PrimaryNavigationProps> = ({
  user,
}) => {
  return (
    <Menu secondary className="top-navigation">
      <Menu.Item as={Link} to={routes.root} className="logo-menu-item">
        <Image size="mini" src={logo} alt="LDWF logo" className="logo" />
      </Menu.Item>
      <DesktopNavigation />
      <Menu.Menu position="right">
        <Dropdown
          item
          className="user-icon"
          trigger={
            <span
              className="user-icon-initial"
              title={`${user.firstName} ${user.lastName}`}
            >
              {user.firstName.substring(0, 1).toUpperCase()}
              {user.lastName.substring(0, 1).toUpperCase()}
            </span>
          }
          icon={null}
        >
          <Dropdown.Menu>
            <Dropdown.Item
              onClick={async () => {
                logoutUser();
              }}
            >
              Sign Out
            </Dropdown.Item>
          </Dropdown.Menu>
        </Dropdown>
      </Menu.Menu>
    </Menu>
  );
};
