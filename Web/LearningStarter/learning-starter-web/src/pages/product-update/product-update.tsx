import { Button, Container, Input, Text } from "@mantine/core";
import { useForm } from "@mantine/form";
import { showNotification } from "@mantine/notifications";
import { useParams } from "react-router-dom";
import api from "../../config/axios";
import {
  ApiResponse,
  ProductCreateDto,
  ProductGetDto,
} from "../../constants/types";
import { ProductFormFields, ProductFormProvider, useProductForm } from "../products/product-form-fields";

export const ProductUpdate = () => {
  const {id} = useParams();

  const mantineForm = useProductForm({
    initialValues: {
      name: "",
      description: "",
    },
    validate: {
      name: (value) => (value.length <= 0 ? "Name must not be empty" : null),
      description: (value) =>
        value.length <= 0 ? "Description must not be empty" : null,
    },
  });

  const submitProduct = async (data: ProductCreateDto) => {
    const result = await api.put<ApiResponse<ProductGetDto>>(
      "/api/products",
      data
    );

    if (result.data.hasErrors) {
      showNotification({ message: "Did Not Update Product!", color: "red" });
    }

    if (result.data.data) {
      showNotification({
        message: "Successfully updated Product!",
        color: "green",
      });
      
    }
  };

  return (
    <ProductFormProvider form={mantineForm}>
      <form onSubmit={mantineForm.onSubmit(submitProduct)}>
        <Container>
          <ProductFormFields />
          <Container px={0}>
            <Button type="submit">Create</Button>
          </Container>
        </Container>
      </form>
    </ProductFormProvider>
  );
};
