import { Center, Header, Table, Text } from "@mantine/core";
import { useEffect, useState } from "react";
import api from "../../config/axios";
import { ApiResponse, ProductGetDto } from "../../constants/types";

export const TestPage = () => {
  const [products, setProducts] = useState<ProductGetDto[]>();
  useEffect(() => {
    fetchProducts();

    async function fetchProducts() {
      var result = await api.get<ApiResponse<ProductGetDto[]>>("/api/products");
      console.log(result.data);
      setProducts(result.data.data);
    }
  }, []);

  return (
    <>
      <Center>
        <Header height={24}>Products</Header>
      </Center>
      <div>
        <Center>
          {products && (
            <Table withBorder striped>
              <thead>
                <tr>
                  <th>Name</th>
                  <th>Description</th>
                </tr>
              </thead>
              <tbody>
                {products.map((product) => (
                  <tr key={product.id}>
                    <td>{product.name}</td>
                    <td>{product.description}</td>
                  </tr>
                ))}
              </tbody>
            </Table>
          )}
        </Center>
      </div>
    </>
  );
};
