import { Container, Input, TextInput } from "@mantine/core"
import { createFormContext } from "@mantine/form";

type ProductFormValues = {
    name: string;
    description: string;
}

type Foo<T> = {
    bar: T
}

const fizz : Foo<number> = {
    bar: 1234
}

const test : Foo<string> = {
    bar: "1234"
}

export const [ProductFormProvider, useProductFormContext, useProductForm] = createFormContext<ProductFormValues>();

export const ProductFormFields = () => {
    const form = useProductFormContext();
    return (
        <>
        <Container>
            <TextInput label="Name" required {...form.getInputProps("name")} />
        </Container>
        <Container>
            <TextInput label="Description" required {...form.getInputProps("description")} />
        </Container>
      </>
    )
}