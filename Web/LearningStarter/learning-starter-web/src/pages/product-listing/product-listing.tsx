import { faPencil } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Button, Container, createStyles, Table, Image } from "@mantine/core";
import { showNotification } from "@mantine/notifications";
import { useEffect, useState } from "react"
import { useNavigate } from "react-router-dom";
import { useUser } from "../../authentication/use-auth";
import api from "../../config/axios";
import { ApiResponse, ProductGetDto } from "../../constants/types";
import { routes } from "../../routes";

export const ProductListing = () => {
    const navigate = useNavigate();
    const {classes} = useStyles();
    const [products, setProducts] = useState<ProductGetDto[]>()
    
    useEffect(() => {
        fetchProducts();

        async function fetchProducts() {
            const {data: response} = await api.get<ApiResponse<ProductGetDto[]>>('/api/products');

            if(response.hasErrors){
                showNotification({message: "error fetching products", color: "red"})
                return;
            }

            setProducts(response.data)
        }
    }, [])

    return (<Container>

    {products && (
        <>
        <Button onClick={() => navigate(routes.productCreate)}>Create</Button>
        <Table withBorder striped>
            <thead>
                <tr>
                    <th />
                    <th>
                        Name
                    </th>
                    <th>
                        Description
                    </th>
                </tr>
            </thead>
            <tbody>
                {products.map((product) => {
                    return (
                        <tr>
                            <td>
                               <FontAwesomeIcon className={classes.pointer} icon={faPencil} onClick={() =>
                                 navigate(routes.productUpdate.replace(":id", product.id.toString()))} /> 
                            </td>
                            <td>
                                {product.name}
                            </td>
                            <td>
        <Image maw={240} mx="auto" radius="md" src={product.description} alt="Granny wit a gun" />

                                
                            </td>
                        </tr>
                    )
                })}
            </tbody>
        </Table>
        </>
        )}
    </Container>)
}

const useStyles = createStyles(() => {
    return {
        pointer: {
            cursor: "pointer"
        }
    }
})